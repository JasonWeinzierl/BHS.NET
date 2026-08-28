import { DatePipe } from '@angular/common';
import { ChangeDetectionStrategy, Component, computed, inject, signal } from '@angular/core';
import { rxResource, toSignal } from '@angular/core/rxjs-interop';
import { PermissionsService } from '@core/services/permissions.service';
import { LeadershipService } from '@data/leadership';
import parseErrorMessage from '@shared/parse-error-message';
import { LeadershipEditorComponent } from '../../components/leadership-editor/leadership-editor.component';

@Component({
  selector: 'app-organization',
  templateUrl: './organization.component.html',
  changeDetection: ChangeDetectionStrategy.OnPush,
  imports: [
    DatePipe,
    LeadershipEditorComponent,
  ],
  host: {
    'data-testid': 'Organization',
  },
  styles: [`
    .slide-fade-in {
      animation: slide-fade-in 0.5s ease-in-out;
    }

    @keyframes slide-fade-in {
      0% {
        opacity: 0;
        transform: translateY(-10px);
      }
      100% {
        opacity: 1;
        transform: translateY(0);
      }
    }
  `],
})
export default class OrganizationComponent {
  private readonly leadershipService = inject(LeadershipService);
  private readonly permissionsService = inject(PermissionsService);

  readonly isEditingSignal = signal(false);
  readonly officersResource = rxResource({
    stream: () => this.leadershipService.getOfficers$(),
  });

  readonly directorsResource = rxResource({
    stream: () => this.leadershipService.getDirectors$(),
  });

  readonly officersSignal = computed(() => this.officersResource.hasValue() ? this.officersResource.value() : []);
  readonly directorsSignal = computed(() => this.directorsResource.hasValue() ? this.directorsResource.value() : []);
  readonly isLoadingSignal = computed(() => this.officersResource.isLoading() || this.directorsResource.isLoading());

  readonly errorMessageSignal = computed(() => {
    const error = this.officersResource.error() ?? this.directorsResource.error();
    return error ? parseErrorMessage(error) ?? 'An unknown error occurred.' : undefined;
  });

  readonly isAuthenticatedSignal = toSignal(this.permissionsService.isAuthenticated$);
  readonly canEditSignal = toSignal(
    this.permissionsService.hasPermission$('write:leadership'),
    { initialValue: false },
  );

  onEdit(): void {
    this.isEditingSignal.set(true);
  }

  onCancel(): void {
    this.isEditingSignal.set(false);
  }

  onSaved(): void {
    this.isEditingSignal.set(false);
    this.officersResource.reload();
    this.directorsResource.reload();
  }
}
