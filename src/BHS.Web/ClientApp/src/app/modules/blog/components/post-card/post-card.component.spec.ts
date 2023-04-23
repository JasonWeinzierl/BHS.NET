import { ComponentFixture, TestBed } from '@angular/core/testing';
import { DateComponent } from '@shared/components/date/date.component';
import { PostCardComponent } from './post-card.component';
import { PostPreview } from '@data/blog';
import { RouterLinkDirectiveStub } from '@app/mock-testing-objects';

describe('PostCardComponent', () => {
  let component: PostCardComponent;
  let fixture: ComponentFixture<PostCardComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ PostCardComponent, RouterLinkDirectiveStub, DateComponent ],
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(PostCardComponent);
    component = fixture.componentInstance;
    component.post = {} as PostPreview;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
