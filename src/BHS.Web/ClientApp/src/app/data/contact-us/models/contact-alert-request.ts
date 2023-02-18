export class ContactAlertRequest {
  constructor(
    public name?: string | null,
    public emailAddress?: string,
    public message?: string | null,
    public dateRequested?: Date | null,
    public body?: string | null,
  ) { }
}
