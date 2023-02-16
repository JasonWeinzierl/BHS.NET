import { CategorySummary, PostPreview } from '@app/data/blog';
import { mergePartially, NestedPartial } from 'merge-partially';
import { Album } from '@app/data/photos';
import { faker } from '@faker-js/faker';
import { SiteBanner } from '@app/data/banners';

export const createCategorySummary = (overrides?: NestedPartial<CategorySummary>): CategorySummary => {
  return mergePartially.deep(
    {
      postsCount: faker.datatype.number(),
      slug: faker.lorem.slug(),
      name: faker.lorem.word(),
    },
    overrides,
  );
};

export const createBlogPostPreview = (overrides?: NestedPartial<PostPreview>): PostPreview => {
  return mergePartially.deep(
    {
      slug: faker.internet.domainWord(),
      title: faker.lorem.word(),
      contentPreview: faker.lorem.paragraph(),
      datePublished: faker.date.past(),
      author: {
        username: faker.internet.userName(),
        name: faker.name.fullName(),
      },
      categories: [
        {
          slug: faker.internet.domainWord(),
          name: faker.lorem.word(),
        },
      ],
    },
    overrides,
  );
};

export const createBanner = (overrides?: NestedPartial<SiteBanner>): SiteBanner => {
  return mergePartially.deep(
    {
      theme: faker.datatype.number({ max: 6 }),
      lead: faker.lorem.sentence(),
      body: faker.lorem.sentence(),
    } as SiteBanner,
    overrides,
  );
};

export const createPhotoAlbum = (overrides?: NestedPartial<Album>): Album=> {
  return mergePartially.deep(
    {
      slug: faker.internet.domainWord(),
      name: faker.lorem.word(),
      description: faker.lorem.sentence(),
      author: {
        username: faker.internet.userName(),
        name: faker.name.fullName(),
      },
    },
    overrides,
  );
};
