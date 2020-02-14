import { Component, OnInit, Output, EventEmitter } from '@angular/core';
import { AccountService } from 'src/app/start-page/account.service';
import { TagsServiceService } from './tags-service.service';


@Component({
  selector: 'app-popular-tag-cloud',
  templateUrl: './popular-tag-cloud.component.html',
  styleUrls: ['./popular-tag-cloud.component.sass']
})
export class PopularTagCloudComponent implements OnInit {

  topTags;
  postList;
  selectedTag;
  @Output() outPostLost = new EventEmitter();

  constructor(
    private accountService: AccountService,
    private tagService: TagsServiceService
  ) { }

  ngOnInit() {
    this.tagService.getDataTopTags().subscribe(data => this.topTags = data,
      err => console.log('HTTP error', err.message));
  }

  onSelect(id, tag): void {
    if(this.selectedTag === tag)
    {
      this.selectedTag = null;
      this.tagService.getDataAllPosts().subscribe(data =>{
        this.postList = data;
        this.outPostLost.emit(this.postList);
      },
        err => console.log('HTTP error', err.message));
    }
    else{
      this.selectedTag = tag;
      this.tagService.getDataPostByTags(id).subscribe(data =>{
        this.postList = data;
        this.outPostLost.emit(this.postList.posts);
      },
        err => console.log('HTTP error', err.message));
    }
  }
}
