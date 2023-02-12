import { ComponentFixture, TestBed, waitForAsync } from '@angular/core/testing';
import { Component } from '@angular/core';
import { ContentLayoutComponent } from './content-layout.component';
import { RouterOutletDirectiveStub } from '@app/mock-testing-objects';

@Component({
  selector: 'app-header',
})
// eslint-disable-next-line @angular-eslint/component-class-suffix
class HeaderComponentStub { }

@Component({
  selector: 'app-footer',
})
// eslint-disable-next-line @angular-eslint/component-class-suffix
class FooterComponentStub { }

describe('ContentLayoutComponent', () => {
  let component: ContentLayoutComponent;
  let fixture: ComponentFixture<ContentLayoutComponent>;

  beforeEach(waitForAsync(() => {
    TestBed.configureTestingModule({
      declarations: [ ContentLayoutComponent, HeaderComponentStub, FooterComponentStub, RouterOutletDirectiveStub ],
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ContentLayoutComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
