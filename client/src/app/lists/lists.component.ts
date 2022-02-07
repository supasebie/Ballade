import { Component, OnInit } from '@angular/core';
import { PageEvent } from '@angular/material/paginator';
import { Member } from '../_models/member';
import { Pagination } from '../_models/pagination';
import { User } from '../_models/user';
import { UserParams } from '../_models/userParams';
import { MembersService } from '../_services/members.service';

@Component({
  selector: 'app-lists',
  templateUrl: './lists.component.html',
  styleUrls: ['./lists.component.scss']
})
export class ListsComponent implements OnInit {
  members: Partial<Member[]>;
  userParams: UserParams;
  user: User;
  pagination: Pagination;

  constructor(private memberService: MembersService) {
    this.userParams = this.memberService.getUserParams();
  }

  ngOnInit(): void {
    this.getLikes();
  }

  getLikes() {
    console.log(this.userParams);
    return this.memberService
      .getLikes(this.userParams)
      .subscribe((response) => {
        this.members = response.result;
        this.pagination = response.pagination;
      });
  }

  resetFilters() {
    this.userParams = this.memberService.resetUserParams();
    this.getLikes();
  }

  pageChanged(event: PageEvent) {
    this.userParams.pageNumber = event.pageIndex;
    this.memberService.setUserParams(this.userParams);
    this.getLikes();
  }
}
