import { ComponentFixture, TestBed } from '@angular/core/testing';
import { RouterModule } from '@angular/router';
import { MockComponent } from 'ng-mocks';
import { FooterComponent } from '../footer/footer.component';
import { HeaderComponent } from '../header/header.component';
import { ContentLayoutComponent } from './content-layout.component';

// TODO: re-enable when ng-mocks supports Angular 19.
describe.skip('ContentLayoutComponent', () => {
  let component: ContentLayoutComponent;
  let fixture: ComponentFixture<ContentLayoutComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [
        RouterModule,
        ContentLayoutComponent,
        MockComponent(HeaderComponent),
        MockComponent(FooterComponent),
      ],
    })
    .compileComponents();

    fixture = TestBed.createComponent(ContentLayoutComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
