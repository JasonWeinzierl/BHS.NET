import { ComponentFixture, TestBed } from '@angular/core/testing';
import { By } from '@angular/platform-browser';
import { CategoriesListViewComponent } from './categories-list-view.component';
import { RouterLink } from '@angular/router';
import { RouterTestingModule } from '@angular/router/testing';

describe('CategoriesListViewComponent', () => {
  let component: CategoriesListViewComponent;
  let fixture: ComponentFixture<CategoriesListViewComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [
        RouterTestingModule,
      ],
      declarations: [
        CategoriesListViewComponent,
      ],
    })
    .compileComponents();

    fixture = TestBed.createComponent(CategoriesListViewComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should show loading', () => {
    expect(component.isLoading)
      .withContext('off at first')
      .toBeFalse();

    fixture.componentRef.setInput('isLoading', true);
    fixture.detectChanges();

    const element = fixture.nativeElement as HTMLElement;

    expect(component.isLoading).toBeTrue();
    expect(element.querySelector('.list-group .placeholder-glow'))
      .withContext('should show placeholder')
      .toBeTruthy();
  });

  it('should show error', () => {
    expect(component.error)
      .withContext('no error at first')
      .toBeUndefined();

      fixture.componentRef.setInput('error', 'An error occurred.');
      fixture.detectChanges();

      const element = fixture.nativeElement as HTMLElement;

      expect(component.error).toBeDefined();
      expect(element.querySelector('.list-group .list-group-item-danger'))
        .withContext('should show danger banner')
        .toBeTruthy();
  });

  it('should show each category', () => {
    expect(component.categories)
      .withContext('empty at first')
      .toHaveSize(0);

    fixture.componentRef.setInput('categories', [{
      slug: 'newsletters',
      name: 'Newsletters',
      postsCount: 1,
    }, {
      slug: 'stories',
      name: 'Stories',
      postsCount: 3,
    }]);
    fixture.detectChanges();

    const element = fixture.nativeElement as HTMLElement;

    expect(component.categories).toHaveSize(2);
    expect(element.querySelector('.list-group')?.children)
      .withContext('should list each category')
      .toHaveSize(2);

    const linkDebugElements = fixture.debugElement.queryAll(By.directive(RouterLink));
    const routerLinks = linkDebugElements.map(de => de.injector.get(RouterLink));

    expect(routerLinks).toHaveSize(2);
    expect(routerLinks[0].href).toBe('/apps/blog/category/newsletters');
    expect(routerLinks[1].href).toBe('/apps/blog/category/stories');
  });
});
