import { faker } from '@faker-js/faker';
import { mergePartially, NestedPartial } from 'merge-partially';
import { AlertTheme, SiteBanner } from '@app/data/banners';
import { CategorySummary, PostPreview } from '@app/data/blog';
import { Album } from '@app/data/photos';

export const createCategorySummary = (overrides?: NestedPartial<CategorySummary>): CategorySummary => {
  return mergePartially.deep(
    {
      postsCount: faker.number.int(),
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
        name: faker.person.fullName(),
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
  // Cannot import ALERT_THEMES (or any object) from our Angular app due to compilation issues.
  const alertThemes: Array<AlertTheme> = [
    'None',
    'Primary',
    'Secondary',
    'Success',
    'Danger',
    'Warning',
    'Info',
  ];
  return mergePartially.deep(
    {
      theme: faker.helpers.arrayElement(alertThemes),
      lead: faker.lorem.sentence(),
      body: faker.lorem.sentence(),
    } as SiteBanner,
    overrides,
  );
};

export const createPhotoAlbum = (overrides?: NestedPartial<Album>): Album => {
  return mergePartially.deep(
    {
      slug: faker.internet.domainWord(),
      name: faker.lorem.word(),
      description: faker.lorem.sentence(),
      author: {
        username: faker.internet.userName(),
        name: faker.person.fullName(),
      },
    },
    overrides,
  );
};
