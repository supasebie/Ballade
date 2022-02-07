import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { Message } from '../_models/message';
import { MessageParams } from '../_models/messageParams';
import { getPaginatedResult, getPaginationHeaders } from './paginationHelper';

@Injectable({
  providedIn: 'root'
})
export class MessageService {
  baseUrl = environment.apiUrl;
  message: Message;
  messages: Message[];

  constructor(private http: HttpClient) {}

  getMessagesForUser(messageParams: MessageParams) {
    let params = getPaginationHeaders(
      messageParams.pageNumber,
      messageParams.pageSize
    );
    params = params.append('container', messageParams.container);

    return getPaginatedResult<Message[]>(
      this.baseUrl + 'messages',
      params,
      this.http
    );
  }

  getMessageThread(username: string) {
    return this.http.get<Message[]>(
      this.baseUrl + 'messages/thread/' + username
    );
  }

  postMessage(messageParams: MessageParams) {
    return this.http.post<Message>(this.baseUrl + 'messages', messageParams);
  }

  deleteMessage(id: number) {
    return this.http.delete(this.baseUrl + 'messages/' + id).subscribe(() => {
      this.messages.splice(
        this.messages.findIndex((m) => m.id === id),
        1
      );
    });
  }
}
