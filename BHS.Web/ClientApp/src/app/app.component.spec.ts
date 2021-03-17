import { TestBed } from '@angular/core/testing';
import { RouterTestingModule } from '@angular/router/testing';
import { AppComponent } from '@app/app.component';

describe('AppComponent', () => {
  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [
        RouterTestingModule
      ],
      declarations: [
        AppComponent
      ]
    })
    .compileComponents();
  });

  it('should create the app', () => {
      const fixture = TestBed.createComponent(AppComponent);
      const app = fixture.componentInstance;
      expect(app).toBeTruthy();
    }
  );

  // it('should render title in an h1 tag', () => {
  //     const fixture = TestBed.createComponent(AppComponent);
  //     fixture.detectChanges();
  //     const compiled = fixture.nativeElement;
  //     expect(compiled.querySelector('h1').textContent).toContain(
  //       'Welcome!'
  //     );
  //   }
  // );
});
