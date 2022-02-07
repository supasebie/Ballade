import { Injectable } from '@angular/core';
import { Resolve, ActivatedRouteSnapshot } from '@angular/router';
import { Observable, of } from 'rxjs';
import { Message } from '../_models/message';
import { PaginatedResult } from '../_models/pagination';
import { MessageService } from '../_services/message.service';

@Injectable({
  providedIn: 'root'
})
export class MessagesResolver implements Resolve<PaginatedResult<Message[]>> {
  constructor(private messageService: MessageService) {}
  resolve(): Observable<PaginatedResult<Message[]>> {
    return this.messageService.getMessagesForUser(
      JSON.parse(localStorage.getItem('username'))
    );
  }
}
