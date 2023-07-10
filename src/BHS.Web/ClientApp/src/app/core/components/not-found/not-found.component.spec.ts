import { ComponentFixture, TestBed } from '@angular/core/testing';
import { ActivatedRoute } from '@angular/router';
import { MockProvider } from 'ng-mocks';
import { NotFoundComponent } from './not-found.component';
import { of } from 'rxjs';
import { RouterTestingModule } from '@angular/router/testing';

describe('NotFoundComponent', () => {
  let component: NotFoundComponent;
  let fixture: ComponentFixture<NotFoundComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [
        RouterTestingModule,
      ],
      declarations: [
        NotFoundComponent,
      ],
      providers: [
        MockProvider(ActivatedRoute, {
          'data': of({
            closestPath: '123/456',
          }),
        }),
      ],
    })
    .compileComponents();

    fixture = TestBed.createComponent(NotFoundComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should suggest a path', () => {
    const element = fixture.nativeElement as HTMLElement;

    expect(element.textContent).toContain('Were you looking for the "123/456" page?');
  });
});
