import Page from './page';

class LocationPage extends Page {
  get self() {
    return this.getByTestID('Location');
  }

  get scheduleLoading() {
    return this.getByTestID('Location-Schedule-Loading');
  }

  get scheduleContainer() {
    return this.getByTestID('Location-Schedule-Container');
  }

  get scheduleError() {
    return this.getByTestID('Location-Schedule-Error');
  }
}
export default new LocationPage();
