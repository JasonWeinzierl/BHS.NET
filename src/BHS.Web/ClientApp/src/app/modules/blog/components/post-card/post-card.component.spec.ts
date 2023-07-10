import { ComponentFixture, TestBed } from '@angular/core/testing';
import { DateComponent } from '@shared/components/date/date.component';
import { PostCardComponent } from './post-card.component';
import { RouterTestingModule } from '@angular/router/testing';

describe('PostCardComponent', () => {
  let component: PostCardComponent;
  let fixture: ComponentFixture<PostCardComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ RouterTestingModule ],
      declarations: [ PostCardComponent, DateComponent ],
    })
    .compileComponents();

    fixture = TestBed.createComponent(PostCardComponent);
    component = fixture.componentInstance;
    component.post = {
      slug: '123',
      title: 'Test 123',
      contentPreview: 'Test',
      categories: [],
      datePublished: new Date(),
    };
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
