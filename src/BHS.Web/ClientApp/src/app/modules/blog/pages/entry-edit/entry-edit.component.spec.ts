import { ActivatedRoute, convertToParamMap } from '@angular/router';
import { ComponentFixture, TestBed } from '@angular/core/testing';
import { FormBuilder, ReactiveFormsModule } from '@angular/forms';
import { BlogService } from '@data/blog';
import { EntryEditComponent } from './entry-edit.component';
import { HttpClientTestingModule } from '@angular/common/http/testing';
import { of } from 'rxjs';

describe('EntryEditComponent', () => {
  let component: EntryEditComponent;
  let fixture: ComponentFixture<EntryEditComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [
        HttpClientTestingModule,
        ReactiveFormsModule,
      ],
      declarations: [
        EntryEditComponent,
      ],
      providers: [
        BlogService,
        FormBuilder,
        {
          provide: ActivatedRoute,
          useValue: {
            'paramMap': of(convertToParamMap({
              slug: '123',
            })),
          },
        },
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
