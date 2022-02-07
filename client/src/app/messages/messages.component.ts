import { Component, OnInit } from '@angular/core';
import { MessageService } from '../_services/message.service';
import { Message } from '../_models/message';
import { Pagination } from '../_models/pagination';
import { MessageParams } from '../_models/messageParams';
import { PageEvent } from '@angular/material/paginator';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-messages',
  templateUrl: './messages.component.html',
  styleUrls: ['./messages.component.scss']
})
export class MessagesComponent implements OnInit {
  messages: Message[] = [];
  pagination: Pagination;
  messageParams: MessageParams;
  loading = false;
  constructor(
    private messageService: MessageService,
    private route: ActivatedRoute
  ) {
    this.messageParams = new MessageParams();
  }

  ngOnInit(): void {
    this.loadMessages();
  }

  loadMessages(pageIndex?: number) {
    if (pageIndex == 0) {
      this.messageParams.pageNumber = 0;
    }
    this.loading = true;
    this.messageService
      .getMessagesForUser(this.messageParams)
      .subscribe((response) => {
        this.messages = response.result;
        this.pagination = response.pagination;
        this.loading = false;
      });
  }

  pageChanged(event: PageEvent) {
    if (this.messageParams.pageNumber != event.pageIndex) {
      this.messageParams.pageNumber = event.pageIndex;
      this.loadMessages();
    }
  }

  deleteMessage(id: number) {
    this.messageService.deleteMessage(id);
  }
}
