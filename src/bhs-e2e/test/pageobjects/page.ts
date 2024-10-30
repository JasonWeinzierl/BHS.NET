export default class Page {
  open(path: string) {
    return browser.url(`https://beltonhistoricalsociety.org/${path}`);
  }
}
