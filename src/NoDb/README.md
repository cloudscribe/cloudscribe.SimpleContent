# NoDb - a file system storage for low cost document or data persistance

## Goals

In my SimpleContent project I had very specific ideas about how best to specifically persist blog posts, organizing them into year/month folders according to publication date. This made it easy to query posts from the file system by year or month. Therefore I believe Posts deserved optimal fs storage decisions rather than a generic approach.

But for other objects that I want to persist without a database, I do want a generic solution, and that is what the NoDb project is about. The name NoDb is inspired by the NoSql movement which uses non-relational document storage such as MongoDb or DocumentDb. I think for small projects it is reasonable to do document and object storage similar to a NoSql approach but using the file system and not requiring any separate hosted service.

My basic plan is to serialize to json, group into folders by type so that we can easily query by type. Files will be named [key].json, so we will only be able to query by key or by file properties such as lastwritedate. This project is only for scenarios where those limitations are acceptable, for things that need more complex querying then probably that should use a custom storage plan with domain specific folder arrangements that make it easy to query things. A generic storage pattern will be like this:

    [configurable-base-folder]/[project-name]/[type]/[key].json

Of course, for small data sets you can also load all the data into memory and query the actual objects any way you like with LINQ. So querying the file system storage is a separate concept vs querying object in memory that have already been retrieved from the file system. For cases where there are lots of items, and especially if many/most items are infrequently needed, then loading all of that type into memory may not always be feasible or the best idea. In that case it is better to retrieve it by key when needed.

For now, my goals are limited to implementing what I need for my own projects.

## Non-Goals

This is not an eterprise storage solution. For certain scenarios it can be very scalable, but it is not scalable for all scenarios and not well suited for very large datasets, with concurrent editing. It is ideally suited for small projects, personal or brochure web applications.


  
  