import { ActivatedRoute, convertToParamMap } from '@angular/router';
import { ComponentFixture, TestBed } from '@angular/core/testing';
import { EMPTY, of } from 'rxjs';
import { MockComponent, MockProvider } from 'ng-mocks';
import { AlertComponent } from 'ngx-bootstrap/alert';
import { AuthService } from '@auth0/auth0-angular';
import { BlogService } from '@data/blog';
import { EntryEditComponent } from './entry-edit.component';

describe('EntryEditComponent', () => {
  let component: EntryEditComponent;
  let fixture: ComponentFixture<EntryEditComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [
        EntryEditComponent,
        MockComponent(AlertComponent),
      ],
      providers: [
        MockProvider(BlogService, {
          getPost: () => EMPTY,
          getCategories: () => EMPTY,
          updatePost: () => EMPTY,
        }),
        MockProvider(ActivatedRoute, {
          'paramMap': of(convertToParamMap({
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
