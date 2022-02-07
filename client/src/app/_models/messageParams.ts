import { BaseParams } from './baseParams';

export const ContainerType = {
  Inbox: 'inbox',
  Outbox: 'outbox',
  Unread: 'unread'
} as const;

export class MessageParams extends BaseParams {
  container: string;
  content: string;
  recipientUsername: string;

  constructor() {
    super();
    this.container = ContainerType.Unread;
  }
}
