import { Component, Input, OnInit, ViewChild } from '@angular/core';
import { NgForm } from '@angular/forms';
import { ActivatedRoute } from '@angular/router';
import { Message } from 'src/app/_models/message';
import { MessageParams } from 'src/app/_models/messageParams';
import { MessageService } from 'src/app/_services/message.service';

@Component({
  selector: 'app-member-messages',
  templateUrl: './member-messages.component.html',
  styleUrls: ['./member-messages.component.scss']
})
export class MemberMessagesComponent implements OnInit {
  @ViewChild('messageForm') messageForm: NgForm;
  @Input() messages: Message[];
  username: string;
  messageParams: MessageParams;
  constructor(
    private messageService: MessageService,
    private route: ActivatedRoute
  ) {
    this.messageParams = new MessageParams();
  }

  ngOnInit(): void {}

  sendMessage() {
    this.messageParams.recipientUsername =
      this.route.snapshot.paramMap.get('username');
    return this.messageService
      .postMessage(this.messageParams)
      .subscribe((message) => {
        this.messages.push(message);
        this.messageForm.reset();
      });
  }
}
