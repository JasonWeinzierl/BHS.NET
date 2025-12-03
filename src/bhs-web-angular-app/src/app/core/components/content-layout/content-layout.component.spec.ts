import { Component } from '@angular/core';
import { ComponentFixture, TestBed } from '@angular/core/testing';
import { RouterModule, RouterOutlet } from '@angular/router';
import { ContentLayoutComponent } from './content-layout.component';

@Component({
  selector: 'app-header',
  template: '',
})
class HeaderStubComponent {}

@Component({
  selector: 'app-footer',
  template: '',
})
class MockFooterComponent {}

describe('ContentLayoutComponent', () => {
  let component: ContentLayoutComponent;
  let fixture: ComponentFixture<ContentLayoutComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [
        RouterModule,
        ContentLayoutComponent,
      ],
    })
    .overrideComponent(ContentLayoutComponent, {
      set: {
        imports: [
          HeaderStubComponent,
          MockFooterComponent,
          RouterOutlet,
        ],
      },
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
