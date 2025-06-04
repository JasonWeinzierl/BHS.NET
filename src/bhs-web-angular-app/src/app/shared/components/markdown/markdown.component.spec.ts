import { ComponentFixture, TestBed } from '@angular/core/testing';
import { MarkdownComponent } from './markdown.component';

describe('MarkdownComponent', () => {
  let component: MarkdownComponent;
  let fixture: ComponentFixture<MarkdownComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [
        MarkdownComponent,
      ],
    })
    .compileComponents();

    fixture = TestBed.createComponent(MarkdownComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should render markdown content', () => {
    const testMarkdown = '# Hello World\nThis is a **test**.';
    fixture.componentRef.setInput('data', testMarkdown);
    fixture.detectChanges();

    const compiled = fixture.nativeElement as HTMLElement;

    expect(compiled.querySelector('h1')?.textContent).toContain('Hello World');
    expect(compiled.querySelector('strong')?.textContent).toContain('test');
  });

  it('should render images', () => {
    const testMarkdown = '![Alt text](https://example.com/image.jpg "Image Title")';
    fixture.componentRef.setInput('data', testMarkdown);
    fixture.detectChanges();

    const compiled = fixture.nativeElement as HTMLElement;
    const img = compiled.querySelector('img');

    expect(img).toBeTruthy();
    expect(img?.getAttribute('src')).toBe('https://example.com/image.jpg');
  });
});
