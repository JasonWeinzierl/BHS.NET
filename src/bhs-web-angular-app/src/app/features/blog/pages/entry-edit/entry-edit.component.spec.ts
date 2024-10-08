import { ComponentFixture, TestBed } from '@angular/core/testing';
import { ActivatedRoute, convertToParamMap } from '@angular/router';
import { AuthService } from '@auth0/auth0-angular';
import { MockProvider } from 'ng-mocks';
import { EMPTY, of } from 'rxjs';
import { EntryEditComponent } from './entry-edit.component';
import { BlogService } from '@data/blog';

describe('EntryEditComponent', () => {
  let component: EntryEditComponent;
  let fixture: ComponentFixture<EntryEditComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [
        EntryEditComponent,
      ],
      providers: [
        MockProvider(BlogService, {
          getPost: () => EMPTY,
          getCategories: () => EMPTY,
          updatePost: () => EMPTY,
        }),
        MockProvider(ActivatedRoute, {
          paramMap: of(convertToParamMap({
            slug: '123',
          })),
        }),
        MockProvider(AuthService, {
          user$: EMPTY,
        }),
      ],
    })
    .compileComponents();

    fixture = TestBed.createComponent(EntryEditComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
