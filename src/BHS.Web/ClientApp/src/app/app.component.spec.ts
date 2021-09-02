import { Observable, of } from 'rxjs';
import { Pipe, PipeTransform } from '@angular/core';
import { AppComponent } from '@app/app.component';
import { Key } from '@service-work/is-loading/is-loading/is-loading.service';
import { RouterTestingModule } from '@angular/router/testing';
import { TestBed } from '@angular/core/testing';

@Pipe({ name: 'swIsLoading' })
class MockIsLoadingPipe implements PipeTransform {
  isLoading = false;

  transform(key: Key): Observable<boolean> {
    return of(this.isLoading);
  }
}

describe('AppComponent', () => {
  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [
        RouterTestingModule
      ],
      declarations: [
        AppComponent,
        MockIsLoadingPipe,
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
