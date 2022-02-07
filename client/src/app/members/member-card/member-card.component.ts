import { Component, Input, OnInit } from '@angular/core';
import { MatSnackBar } from '@angular/material/snack-bar';
import { Member } from 'src/app/_models/member';
import { MembersService } from 'src/app/_services/members.service';

@Component({
  selector: 'app-member-card',
  templateUrl: './member-card.component.html',
  styleUrls: ['./member-card.component.scss']
})
export class MemberCardComponent implements OnInit {
  @Input() member: Member;

  constructor(
    private memberService: MembersService,
    private snack: MatSnackBar
  ) {}

  ngOnInit(): void {}

  addLike(username: string) {
    this.memberService
      .addLike(username)
      .subscribe((response) => this.snack.open('User liked', 'Close'));
  }
}
