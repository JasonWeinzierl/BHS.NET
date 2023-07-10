import { ComponentFixture, TestBed } from '@angular/core/testing';
import { AuthorService } from '@data/authors';
import { EMPTY } from 'rxjs';
import { MembersListComponent } from './members-list.component';
import { MockProvider } from 'ng-mocks';

describe('MembersListComponent', () => {
  let component: MembersListComponent;
  let fixture: ComponentFixture<MembersListComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [
        MembersListComponent,
      ],
      providers: [
        MockProvider(AuthorService, {
          getAuthors: () => EMPTY,
        }),
      ],
    })
    .compileComponents();

    fixture = TestBed.createComponent(MembersListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
