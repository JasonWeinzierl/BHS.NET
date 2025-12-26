export default class Page {
  protected open(path: string) {
    return browser.url(path);
  }

  protected getByTestID(testID: string) {
    return browser.$(`[data-testid="${testID}"]`);
  }
}
