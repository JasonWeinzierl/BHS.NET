import { ComponentFixture, TestBed } from '@angular/core/testing';
import { AuthService } from '@auth0/auth0-angular';
import { BlogService } from '@data/blog';
import { EMPTY } from 'rxjs';
import { EntryNewComponent } from './entry-new.component';
import { MockProvider } from 'ng-mocks';
import { RouterTestingModule } from '@angular/router/testing';

describe('EntryNewComponent', () => {
  let component: EntryNewComponent;
  let fixture: ComponentFixture<EntryNewComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [ RouterTestingModule ],
      declarations: [EntryNewComponent],
      providers: [
        MockProvider(BlogService, {
          getCategories: () => EMPTY,
        }),
        MockProvider(AuthService, {
          user$: EMPTY,
        }),
      ],
    });

    fixture = TestBed.createComponent(EntryNewComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
