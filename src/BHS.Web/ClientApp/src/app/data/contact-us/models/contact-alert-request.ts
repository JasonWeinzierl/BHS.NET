export interface ContactAlertRequest {
  name?: string | null,
  emailAddress?: string,
  message?: string | null,
  dateRequested?: Date | null,
  body?: string | null,
}
