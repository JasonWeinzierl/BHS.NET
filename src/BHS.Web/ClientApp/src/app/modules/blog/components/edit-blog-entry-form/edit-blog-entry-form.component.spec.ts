import { ComponentFixture, TestBed } from '@angular/core/testing';
import { FormBuilder, ReactiveFormsModule } from '@angular/forms';
import { EditBlogEntryFormComponent } from './edit-blog-entry-form.component';

describe('EditBlogEntryFormComponent', () => {
  let component: EditBlogEntryFormComponent;
  let fixture: ComponentFixture<EditBlogEntryFormComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [
        ReactiveFormsModule,
      ],
      declarations: [ EditBlogEntryFormComponent ],
      providers: [ FormBuilder ],
    })
    .compileComponents();

    fixture = TestBed.createComponent(EditBlogEntryFormComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
