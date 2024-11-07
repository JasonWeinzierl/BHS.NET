import { Agent, fetch } from 'undici';
import { z } from 'zod';

// TODO: dedupe with web app, define models in a separate package.
const postPreviewSchema = z.object({
  slug: z.string(),
  title: z.string(),
  contentPreview: z.string(),
  author: z.object({
    username: z.string(),
    name: z.string().nullish(),
  }).nullish(),
  datePublished: z.coerce.date(),
});
type PostPreview = z.infer<typeof postPreviewSchema>;

describe('news', () => {
  let firstPost: PostPreview;

  beforeAll(async () => {
    if (!browser.options.baseUrl) {
      throw new Error('baseUrl is not set.');
    }

    const response = await fetch(browser.options.baseUrl + '/api/blog/posts', {
      dispatcher: new Agent({
        connect: {
          rejectUnauthorized: false,
        },
      }),
    });

    await expect(response.status).toBe(200);

    const posts = postPreviewSchema.array().parse(await response.json());
    if (!posts.length) {
      throw new Error('No posts found. Cannot test UI without sample data.');
    }
    posts.sort((a, b) => b.datePublished.getTime() - a.datePublished.getTime());

    firstPost = posts[0];
  });

  it('should navigate to News', async () => {
    await browser.url('/');
    await $('a=News').click();
    await expect($('h1')).toHaveText('News');
  });

  it('should display each post preview', async () => {
    await browser.url('/apps/blog');

    await expect($('app-post-card')).toExist();
    await expect($(`[data-testid="${firstPost.slug}"] .card-title`)).toHaveText(firstPost.title);
    await expect($(`[data-testid="${firstPost.slug}"] .card-body`)).toHaveText(expect.stringContaining(firstPost.contentPreview.replace(/\n/g, ' ')));
    await expect($(`[data-testid="${firstPost.slug}"] .card-subtitle`)).toHaveText(expect.stringContaining(firstPost.datePublished.getFullYear().toString()));
    if (firstPost.author?.name) {
      await expect($(`[data-testid="${firstPost.slug}"] .card-subtitle`)).toHaveText(expect.stringContaining(firstPost.author.name));
    }
  });
});
