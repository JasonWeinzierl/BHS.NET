import appHeaderPage from '../pageobjects/appHeader.page';
import homePage from '../pageobjects/home.page';
import organizationPage from '../pageobjects/organization.page';

describe('leadership', () => {
  beforeAll(async () => {
    await homePage.open();
    await homePage.self.waitForDisplayed();
  });

  it('should navigate to Organization from Home', async () => {
    await appHeaderPage.navbarAboutMenuButton.click();
    await appHeaderPage.navbarByLawsAndLeadershipLink.click();

    await expect(organizationPage.self).toBeDisplayed();
  });

  it('should stop loading', async () => {
    await organizationPage.officersLoading.waitForDisplayed({ reverse: true });
    await organizationPage.directorsLoading.waitForDisplayed({ reverse: true });

    await expect(organizationPage.officersContainer).toBeDisplayed();
    await expect(organizationPage.directorsContainer).toBeDisplayed();
  });
});
