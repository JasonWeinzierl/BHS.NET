import { ComponentFixture, TestBed } from '@angular/core/testing';
import { RouterModule } from '@angular/router';
import { PostCardComponent } from './post-card.component';
import { DateComponent } from '@shared/components/date/date.component';

describe('PostCardComponent', () => {
  let component: PostCardComponent;
  let fixture: ComponentFixture<PostCardComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [
        RouterModule.forRoot([]),
        PostCardComponent,
        DateComponent,
      ],
    })
    .compileComponents();

    fixture = TestBed.createComponent(PostCardComponent);
    component = fixture.componentInstance;
    fixture.componentRef.setInput('post', {
      slug: '123',
      title: 'Test 123',
      contentPreview: 'Test',
      categories: [],
      datePublished: new Date(),
    });
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
