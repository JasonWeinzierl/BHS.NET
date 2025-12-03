import { ComponentFixture, TestBed } from '@angular/core/testing';
import { ActivatedRoute, convertToParamMap } from '@angular/router';
import { EMPTY, of } from 'rxjs';
import { ProfileIndexComponent } from './profile-index.component';
import { AuthorService } from '@data/authors';
import { MockProvider } from 'ng-mocks';

describe('ProfileIndexComponent', () => {
  let component: ProfileIndexComponent;
  let fixture: ComponentFixture<ProfileIndexComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ProfileIndexComponent],
      providers: [
        MockProvider(AuthorService, {
          getAuthorPosts: () => EMPTY,
        }),
        MockProvider(ActivatedRoute, {
          paramMap: of(convertToParamMap({
            username: 'abc',
          })),
        }),
      ],
    })
    .compileComponents();

    fixture = TestBed.createComponent(ProfileIndexComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
