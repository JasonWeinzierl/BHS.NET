import { ComponentFixture, TestBed } from '@angular/core/testing';
import { JsonLdBlogPostingComponent } from './json-ld-blog-posting.component';
import { Post } from '@data/blog';

describe('JsonLdBlogPostingComponent', () => {
  let component: JsonLdBlogPostingComponent;
  let fixture: ComponentFixture<JsonLdBlogPostingComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [JsonLdBlogPostingComponent],
    })
    .compileComponents();

    fixture = TestBed.createComponent(JsonLdBlogPostingComponent);
    component = fixture.componentInstance;
    fixture.componentRef.setInput('post', {
      slug: 'test',
      title: 'Test Post',
      contentMarkdown: 'This is a test post.',
      datePublished: new Date(),
      dateLastModified: new Date(),
      categories: [{ slug: 'test-category', name: 'Test Category' }],
      author: {
        name: 'Test Author',
        username: 'testauthor',
      },
    } satisfies Post);
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should generate valid JSON-LD', () => {
    const jsonLdScriptEl = fixture.nativeElement as HTMLScriptElement;

    expect(jsonLdScriptEl.innerHTML).toContain('<script type="application/ld+json">');

    const jsonLd = JSON.parse(jsonLdScriptEl.textContent) as Record<string, unknown>;

    expect(jsonLd['@context']).toBe('https://schema.org');
    expect(jsonLd['@type']).toBe('BlogPosting');
    expect(jsonLd['headline']).toBe('Test Post');
    expect(jsonLd['name']).toBe('Test Post');
    expect(jsonLd['datePublished']).toBeTypeOf('string');
    expect(jsonLd['dateModified']).toBeTypeOf('string');
    expect(jsonLd['keywords']).toEqual(['Test Category']);
    expect(jsonLd['author']).toEqual(expect.objectContaining({
      name: 'Test Author',
    }));
  });
});
