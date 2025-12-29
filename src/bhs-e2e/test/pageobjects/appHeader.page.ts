import Page from './page';

class AppHeaderPage extends Page {
  get self() {
    return this.getByTestID('AppHeader');
  }

  get navbarToggle() {
    return this.getByTestID('AppHeader-Navbar-ToggleButton');
  }

  get navbarHomeLink() {
    return this.getByTestID('AppHeader-Navbar-HomeLink');
  }

  get navbarNewsLink() {
    return this.getByTestID('AppHeader-Navbar-NewsLink');
  }

  get navbarContentMenuButton() {
    return this.getByTestID('AppHeader-Navbar-ContentMenuButton');
  }

  get navbarPhotosLink() {
    return this.getByTestID('AppHeader-Navbar-PhotosLink');
  }

  get navbarLocationLink() {
    return this.getByTestID('AppHeader-Navbar-LocationLink');
  }

  get navbarContactLink() {
    return this.getByTestID('AppHeader-Navbar-ContactLink');
  }

  get navbarAdminLink() {
    return this.getByTestID('AppHeader-Navbar-AdminLink');
  }

  get navbarAboutMenuButton() {
    return this.getByTestID('AppHeader-Navbar-AboutMenuButton');
  }

  get navbarWhoWeAreLink() {
    return this.getByTestID('AppHeader-Navbar-WhoWeAreLink');
  }

  get navbarByLawsAndLeadershipLink() {
    return this.getByTestID('AppHeader-Navbar-ByLawsAndLeadershipLink');
  }

  get navbarDonateLink() {
    return this.getByTestID('AppHeader-Navbar-DonateLink');
  }

  get navbarAboutThisSiteLink() {
    return this.getByTestID('AppHeader-Navbar-AboutThisSiteLink');
  }

  get navbarTermsOfServiceLink() {
    return this.getByTestID('AppHeader-Navbar-TermsOfServiceLink');
  }

  get navbarPrivacyPolicyLink() {
    return this.getByTestID('AppHeader-Navbar-PrivacyPolicyLink');
  }
}
export default new AppHeaderPage();
