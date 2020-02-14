import { Component, OnInit } from '@angular/core';
import { AccountService } from '../../../start-page/account.service';
import { Route, Router } from '@angular/router';
import { PostsService} from '../../../feed-posts/posts.service';

@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.sass']
})
export class HeaderComponent implements OnInit {
  currentUser;
  isUserLogged: boolean;
  searchItem;
  constructor(
    private accountService: AccountService,
    private route: Router,
    private postService: PostsService ) {

  }

  ngOnInit() {
    this.accountService.GetUserProfile().subscribe(res => {
      this.currentUser = res;
      this.accountService.isUserLogged.next(true);
  });

    this.accountService.isUserLogged.subscribe( value => {
    this.isUserLogged = value;
    });
  }

  LogOut(){
    localStorage.removeItem('token');
    this.route.navigateByUrl('login');
    this.accountService.isUserLogged.next(false);
  }

  searchPosts()  {
    // this.postService.postsSearch(this.searchItem);

  }

}
