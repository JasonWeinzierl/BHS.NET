import { AppPage } from './app.po';

describe('App', () => {
  let page: AppPage;

  beforeEach(() => {
    page = new AppPage();
  });

  it('should display welcome message', () => {
    page.navigateTo();

    expect(page.getMainHeading()).toEqual('The City Hall Museum sponsored by the Belton Historical Society');
  });
});
