import { ComponentFixture, TestBed } from '@angular/core/testing';
import { MockProvider } from 'ng-mocks';
import { EMPTY, of } from 'rxjs';
import { LeadershipService } from '@data/leadership';
import { LeadershipEditorComponent } from './leadership-editor.component';

const OFFICES = [
  { title: 'President', sortOrder: 1 },
];
const DIRECTORS = [
  { name: 'Jane Doe', year: '2026' },
];

describe('LeadershipEditorComponent', () => {
  let fixture: ComponentFixture<LeadershipEditorComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [LeadershipEditorComponent],
      providers: [
        MockProvider(LeadershipService, {
          getOffices$: () => of(OFFICES),
          updateOfficers$: () => EMPTY,
          addDirectors$: () => EMPTY,
          deleteDirector$: () => EMPTY,
        }),
      ],
    })
    .compileComponents();

    fixture = TestBed.createComponent(LeadershipEditorComponent);
    fixture.componentRef.setInput('officers', []);
    fixture.componentRef.setInput('directors', DIRECTORS);

    await fixture.whenStable();
  });

  it('should create', () => {
    expect(fixture.componentInstance).toBeTruthy();
  });

  it('should render vacant offices and the initial directors', () => {
    const element = fixture.nativeElement as HTMLElement;

    const officerName = element.querySelector<HTMLInputElement>('#officer-0-name');
    const officerDateStarted = element.querySelector<HTMLInputElement>('#officer-0-date-started');
    const directorName = element.querySelector<HTMLInputElement>('#director-0-name');
    const directorYear = element.querySelector<HTMLInputElement>('#director-0-year');

    expect(officerName?.value).toBe('');
    expect(officerDateStarted?.value).toBe(new Date().toISOString().slice(0, 10));
    expect(directorName?.value).toBe('Jane Doe');
    expect(directorYear?.value).toBe('2026');
  });
});
