# A list of things I haven't done yet

EF storage layer
localization of views
caching of post/page data
recent posts viewcomponent for homepage

implement a way to add a claim for ProjectId in cloudscribe Core integration to easily grant a user edit permissions - maybe make 2 claims, one for blog and one for pages, both using prokjectid as the value

been thinking that there should be an option to redirect from blog index to newest post
post list looks like you are omn the post page but no comment section, just more posts
since you can navigate forward and backward from post detail, the post list seems redundant and confusing to users
ie when a user clicks the blog link in the menu they land on the post list but I think I would prefer if they land on the newest post detail
still I guess the list should always work, so maybe we need a new action for newest post then the menu item can be changed to point to that




## example project stuff

gulp/grunt work for production ie fontawesome copy to folder

latest trend single page site but the blog would be a separate page

### an example that uses a single page for home about contact with scrolling and paralax - try it also for joeaudette.com
for that I think I would just use a single Home Index view and have the content right in the view
code the anchors into the navigation
then still be able to add cms pages in the nav dynamically but the cms would not be the default route in this case
since the home index would be
therefore I will use an url like /p/page-name

once we can package views in nuget it will be easier to setup multiple sample web apps with different configurations

