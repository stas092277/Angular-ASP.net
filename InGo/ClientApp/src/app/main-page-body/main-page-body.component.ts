import { Component, OnInit } from '@angular/core';
import { AccountService } from '../start-page/account.service';
import { Post } from '../post/Itpost';
import { AllMainPostsService } from './all-main-posts.service';

@Component({
  selector: 'app-main-page-body',
  templateUrl: './main-page-body.component.html',
  styleUrls: ['./main-page-body.component.sass']
})
export class MainPageBodyComponent implements OnInit {

  postList: Post[];
  defaultValue = '';
  userDetails;

  constructor(
    private accountService: AccountService,
    private allMainPostsService: AllMainPostsService
  ) { }

  ngOnInit() {
    this.allMainPostsService.getDataAllPosts().subscribe(data => this.postList = data,
      err => console.log('HTTP error', err.message));

    this.accountService.GetUserProfile().subscribe(res => {
      this.userDetails = res;
      // console.log(res);
}, err => { console.log(err); }, ); }

  receivePostFromTag($event: Post[]) {
    this.postList = $event;
  }


}
