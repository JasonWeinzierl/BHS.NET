import { ComponentFixture, TestBed } from '@angular/core/testing';
import { provideRouter } from '@angular/router';
import { EMPTY, Observable, Subject } from 'rxjs';
import { MockInstance, vi } from 'vitest';
import { ContactFormComponent } from './contact-form.component';
import { InsightsService } from '@core/services/insights.service';
import { ContactAlertRequest, ContactService } from '@data/contact-us';
import { MockProvider } from 'ng-mocks';

describe('ContactFormComponent', () => {
  let component: ContactFormComponent;
  let fixture: ComponentFixture<ContactFormComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [
        ContactFormComponent,
      ],
      providers: [
        provideRouter([]),
        MockProvider(ContactService, {
          sendMessage: () => EMPTY,
        }),
        MockProvider(InsightsService, {
          submitContactForm: vi.fn(),
        }),
      ],
    })
    .compileComponents();

    fixture = TestBed.createComponent(ContactFormComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  describe('when sending a message', () => {
    let nameInput: HTMLInputElement | null;
    let emailInput: HTMLInputElement | null;
    let messageInput: HTMLTextAreaElement | null;
    let bodyInput: HTMLInputElement | null;
    let submitButton: HTMLButtonElement | null;
    let sendMessage: MockInstance<(request: ContactAlertRequest) => Observable<void>>;
    let sendMessageResponseSubject$: Subject<void>;

    beforeEach(() => {
      const nativeElement = fixture.nativeElement as HTMLElement;
      nameInput = nativeElement.querySelector('#inputName');
      emailInput = nativeElement.querySelector('#inputEmail');
      messageInput = nativeElement.querySelector('#inputMessage');
      bodyInput = nativeElement.querySelector('#body');
      submitButton = nativeElement.querySelector('button[type="submit"]');
      sendMessageResponseSubject$ = new Subject<void>();
      sendMessage = vi.spyOn(TestBed.inject(ContactService), 'sendMessage').mockImplementation(() => sendMessageResponseSubject$);
    });

    it('should show contact form', () => {
      expect(nameInput).toBeTruthy();
      expect(emailInput).toBeTruthy();
      expect(messageInput).toBeTruthy();
      expect(bodyInput).toBeTruthy();
      expect(submitButton).toBeTruthy();
    });

    it('should send a successful message', () => {
      if (!nameInput || !emailInput || !messageInput || !bodyInput || !submitButton) {
        throw new Error('One or more elements are missing');
      }

      nameInput.value = 'Test Name';
      emailInput.value = 'test@test.com';
      messageInput.value = 'Test Message';
      submitButton.click();
      fixture.detectChanges();

      expect((fixture.nativeElement as HTMLElement).querySelector('.progress')).toBeTruthy();
      expect(sendMessage).toHaveBeenCalled();

      sendMessageResponseSubject$.next();
      fixture.detectChanges();

      expect((fixture.nativeElement as HTMLElement).querySelector('[data-testid="ContactForm-ThankYou"]')?.textContent).toBe('Thank you for your message.');
    });

    it('should show an error on failure', () => {
      if (!nameInput || !emailInput || !messageInput || !bodyInput || !submitButton) {
        throw new Error('One or more elements are missing');
      }

      nameInput.value = 'Test Name';
      emailInput.value = 'test@test.com';
      messageInput.value = 'Test Message';
      submitButton.click();
      fixture.detectChanges();

      expect((fixture.nativeElement as HTMLElement).querySelector('.progress')).toBeTruthy();
      expect(sendMessage).toHaveBeenCalled();

      sendMessageResponseSubject$.error(new Error('Test error'));
      fixture.detectChanges();

      expect((fixture.nativeElement as HTMLElement).querySelector('.alert')?.textContent).toContain('There was an error submitting your message.');
    });

    describe('when the request takes a while', () => {
      beforeEach(() => {
        vi.useFakeTimers();
      });

      afterEach(async () => {
        await vi.runAllTimersAsync();
        vi.useRealTimers();
      });

      it('should timeout after 10 seconds', async () => {
        if (!nameInput || !emailInput || !messageInput || !bodyInput || !submitButton) {
          throw new Error('One or more elements are missing');
        }

        nameInput.value = 'Test Name';
        emailInput.value = 'test@test.com';
        messageInput.value = 'Test Message';
        submitButton.click();
        fixture.detectChanges();

        expect((fixture.nativeElement as HTMLElement).querySelector('.progress')).toBeTruthy();
        expect(sendMessage).toHaveBeenCalled();

        await vi.advanceTimersByTimeAsync(10_000);
        fixture.detectChanges();

        expect((fixture.nativeElement as HTMLElement).querySelector('.progress')).toBeFalsy();
        expect((fixture.nativeElement as HTMLElement).querySelector('.alert')?.textContent).toContain('Something took too long...');
      });

      it('should not timeout after 5 seconds', async () => {
        if (!nameInput || !emailInput || !messageInput || !bodyInput || !submitButton) {
          throw new Error('One or more elements are missing');
        }

        nameInput.value = 'Test Name';
        emailInput.value = 'test@test.com';
        messageInput.value = 'Test Message';
        submitButton.click();
        fixture.detectChanges();

        expect((fixture.nativeElement as HTMLElement).querySelector('.progress')).toBeTruthy();
        expect(sendMessage).toHaveBeenCalled();

        await vi.advanceTimersByTimeAsync(5_000);
        fixture.detectChanges();

        expect((fixture.nativeElement as HTMLElement).querySelector('.progress')).toBeTruthy();
        expect((fixture.nativeElement as HTMLElement).querySelector('.alert')).toBeFalsy();
      });
    });
  });
});
