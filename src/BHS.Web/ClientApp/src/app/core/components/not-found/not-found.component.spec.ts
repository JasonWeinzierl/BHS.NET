import { ComponentFixture, TestBed } from '@angular/core/testing';
import { ActivatedRoute } from '@angular/router';
import { NotFoundComponent } from './not-found.component';
import { of } from 'rxjs';
import { RouterLinkDirectiveStub } from '@app/mock-testing-objects';

describe('NotFoundComponent', () => {
  let component: NotFoundComponent;
  let fixture: ComponentFixture<NotFoundComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [
        NotFoundComponent,
        RouterLinkDirectiveStub,
      ],
      providers: [
        {
          provide: ActivatedRoute,
          useValue: {
            'data': of({
              closestPath: '123/456',
            }),
          },
        },
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
