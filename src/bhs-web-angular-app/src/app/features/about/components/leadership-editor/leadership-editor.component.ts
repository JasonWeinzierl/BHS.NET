import { ChangeDetectionStrategy, Component, computed, DestroyRef, inject, input, linkedSignal, output, signal } from '@angular/core';
import { rxResource, takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { applyEach, form, FormField, required } from '@angular/forms/signals';
import { forkJoin, of } from 'rxjs';
import { Director, DirectorRequest, LeadershipService, Officer, OfficerRequest } from '@data/leadership';
import parseErrorMessage from '@shared/parse-error-message';

interface LeadershipFormModel {
  officers: Array<{
    title: string;
    name: string;
    dateStarted: string;
  }>;
  directors: Array<DirectorRequest>;
}

@Component({
  selector: 'app-leadership-editor',
  templateUrl: './leadership-editor.component.html',
  changeDetection: ChangeDetectionStrategy.OnPush,
  imports: [FormField],
  host: {
    'data-testid': 'Organization-Leadership-Editor',
  },
})
export class LeadershipEditorComponent {
  private readonly leadershipService = inject(LeadershipService);
  private readonly destroyRef = inject(DestroyRef);

  readonly officers = input.required<Array<Officer>>();
  readonly directors = input.required<Array<Director>>();
  readonly saved = output();
  readonly cancelled = output();

  readonly isSubmittingSignal = signal(false);
  readonly errorSignal = signal<string | undefined>(undefined);
  readonly officesResource = rxResource({
    stream: () => this.leadershipService.getOffices$(),
  });

  readonly officesErrorSignal = computed(() => {
    const error = this.officesResource.error();
    return error ? parseErrorMessage(error) ?? 'Unable to load offices.' : undefined;
  });

  readonly editModel = linkedSignal<LeadershipFormModel>(() => {
    const officerByTitle = new Map(this.officers().map(officer => [officer.title, officer]));
    const today = new Date().toISOString().slice(0, 10);
    return {
      officers: (this.officesResource.hasValue() ? this.officesResource.value() : []).map(office => {
        const officer = officerByTitle.get(office.title);
        return {
          title: office.title,
          name: officer?.name ?? '',
          dateStarted: officer?.dateStarted.toISOString().slice(0, 10) ?? today,
        };
      }),
      directors: this.directors().map(director => ({
        name: director.name,
        year: Number(director.year),
      })),
    };
  });

  readonly leadershipForm = form(this.editModel, schemaPath => {
    applyEach(schemaPath.officers, officer => {
      required(officer.dateStarted);
    });
    applyEach(schemaPath.directors, director => {
      required(director.name);
      required(director.year);
    });
  });

  onAddDirector(): void {
    this.editModel.update(model => ({
      ...model,
      directors: [...model.directors, { name: '', year: new Date().getFullYear() }],
    }));
  }

  onRemoveDirector(index: number): void {
    this.editModel.update(model => ({
      ...model,
      directors: model.directors.filter((_, directorIndex) => directorIndex !== index),
    }));
  }

  onSubmit(event: SubmitEvent): void {
    event.preventDefault();
    if (this.leadershipForm().invalid() || this.isSubmittingSignal()) {
      return;
    }

    const model = this.editModel();
    const officerRequests: Array<OfficerRequest> = model.officers.map(officer => ({
      title: officer.title,
      // eslint-disable-next-line unicorn/no-null
      name: officer.name || null,
      dateStarted: new Date(officer.dateStarted),
    }));
    const originalDirectors = this.directors().map(director => ({
      name: director.name,
      year: Number(director.year),
    }));
    const originalDirectorKeys = new Set(originalDirectors.map(director => this.getDirectorKey(director)));
    const currentDirectorKeys = new Set(model.directors.map(director => this.getDirectorKey(director)));
    const additions = model.directors.filter(director => !originalDirectorKeys.has(this.getDirectorKey(director)));
    const deletions = originalDirectors.filter(director => !currentDirectorKeys.has(this.getDirectorKey(director)));

    this.isSubmittingSignal.set(true);
    this.errorSignal.set(undefined);
    forkJoin({
      officers: this.leadershipService.updateOfficers$(officerRequests),
      additions: additions.length > 0 ? this.leadershipService.addDirectors$(additions) : of([]),
      deletions: deletions.length > 0
        ? forkJoin(deletions.map(director => this.leadershipService.deleteDirector$(director)))
        : of([]),
    }).pipe(takeUntilDestroyed(this.destroyRef))
      // eslint-disable-next-line rxjs-angular-x/prefer-async-pipe
      .subscribe({
        next: () => {
          this.isSubmittingSignal.set(false);
          this.saved.emit();
        },
        error: (error: unknown) => {
          this.errorSignal.set(parseErrorMessage(error) ?? 'Unable to update leadership.');
          this.isSubmittingSignal.set(false);
        },
      });
  }

  private readonly getDirectorKey = (director: DirectorRequest): string => JSON.stringify([director.name, director.year]);
}
