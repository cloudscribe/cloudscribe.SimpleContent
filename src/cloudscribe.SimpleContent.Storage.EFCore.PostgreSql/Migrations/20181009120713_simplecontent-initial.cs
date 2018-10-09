using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace cloudscribe.SimpleContent.Storage.EFCore.PostgreSql.Migrations
{
    public partial class simplecontentinitial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "cs_content_history",
                columns: table => new
                {
                    id = table.Column<Guid>(maxLength: 36, nullable: false),
                    content_id = table.Column<string>(maxLength: 36, nullable: false),
                    project_id = table.Column<string>(nullable: true),
                    content_source = table.Column<string>(maxLength: 50, nullable: false),
                    content_type = table.Column<string>(maxLength: 50, nullable: true, defaultValue: "html"),
                    slug = table.Column<string>(maxLength: 255, nullable: true),
                    is_draft_hx = table.Column<bool>(nullable: false),
                    was_deleted = table.Column<bool>(nullable: false),
                    archived_utc = table.Column<DateTime>(nullable: false),
                    archived_by = table.Column<string>(maxLength: 255, nullable: true),
                    title = table.Column<string>(maxLength: 255, nullable: false),
                    author = table.Column<string>(maxLength: 255, nullable: true),
                    correlation_key = table.Column<string>(maxLength: 255, nullable: true),
                    content = table.Column<string>(nullable: true),
                    categories_csv = table.Column<string>(nullable: true),
                    pub_date = table.Column<DateTime>(nullable: true),
                    last_modified = table.Column<DateTime>(nullable: false),
                    is_published = table.Column<bool>(nullable: false),
                    created_utc = table.Column<DateTime>(nullable: false),
                    created_by_user = table.Column<string>(maxLength: 100, nullable: true),
                    last_modified_by_user = table.Column<string>(maxLength: 100, nullable: true),
                    draft_content = table.Column<string>(nullable: true),
                    draft_author = table.Column<string>(maxLength: 255, nullable: true),
                    draft_pub_date = table.Column<DateTime>(nullable: true),
                    serialized_model = table.Column<string>(nullable: true),
                    draft_serialized_model = table.Column<string>(nullable: true),
                    meta_description = table.Column<string>(nullable: true),
                    meta_json = table.Column<string>(nullable: true),
                    meta_html = table.Column<string>(nullable: true),
                    template_key = table.Column<string>(maxLength: 255, nullable: true),
                    serializer = table.Column<string>(maxLength: 50, nullable: true),
                    parent_id = table.Column<string>(maxLength: 255, nullable: true),
                    parent_slug = table.Column<string>(maxLength: 255, nullable: true),
                    page_order = table.Column<int>(nullable: false),
                    view_roles = table.Column<string>(nullable: true),
                    teaser_override = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_cs_content_history", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "cs_content_project",
                columns: table => new
                {
                    id = table.Column<string>(maxLength: 36, nullable: false),
                    title = table.Column<string>(maxLength: 255, nullable: true),
                    description = table.Column<string>(nullable: true),
                    copyright_notice = table.Column<string>(maxLength: 255, nullable: true),
                    publisher = table.Column<string>(maxLength: 255, nullable: true),
                    publisher_logo_url = table.Column<string>(maxLength: 255, nullable: true),
                    publisher_logo_width = table.Column<string>(maxLength: 20, nullable: true),
                    publisher_logo_height = table.Column<string>(maxLength: 20, nullable: true),
                    publisher_entity_type = table.Column<string>(maxLength: 50, nullable: true),
                    disqus_short_name = table.Column<string>(maxLength: 100, nullable: true),
                    posts_per_page = table.Column<int>(nullable: false),
                    days_to_comment = table.Column<int>(nullable: false),
                    moderate_comments = table.Column<bool>(nullable: false),
                    comment_notification_email = table.Column<string>(maxLength: 100, nullable: true),
                    blog_menu_links_to_newest_post = table.Column<bool>(nullable: false),
                    local_media_virtual_path = table.Column<string>(maxLength: 255, nullable: true),
                    cdn_url = table.Column<string>(maxLength: 255, nullable: true),
                    pub_date_format = table.Column<string>(maxLength: 75, nullable: true),
                    include_pub_date_in_post_urls = table.Column<bool>(nullable: false),
                    time_zone_id = table.Column<string>(maxLength: 100, nullable: true),
                    recaptcha_public_key = table.Column<string>(maxLength: 255, nullable: true),
                    recaptcha_private_key = table.Column<string>(maxLength: 255, nullable: true),
                    default_page_slug = table.Column<string>(maxLength: 255, nullable: true),
                    use_default_page_as_root_node = table.Column<bool>(nullable: false),
                    show_title = table.Column<bool>(nullable: false),
                    add_blog_to_pages_tree = table.Column<bool>(nullable: false),
                    blog_page_position = table.Column<int>(nullable: false),
                    blog_page_text = table.Column<string>(maxLength: 255, nullable: true),
                    blog_page_nav_component_visibility = table.Column<string>(maxLength: 255, nullable: true),
                    image = table.Column<string>(maxLength: 255, nullable: true),
                    channel_time_to_live = table.Column<int>(nullable: false),
                    language_code = table.Column<string>(maxLength: 10, nullable: true),
                    channel_categories_csv = table.Column<string>(maxLength: 255, nullable: true),
                    managing_editor_email = table.Column<string>(maxLength: 100, nullable: true),
                    channel_rating = table.Column<string>(maxLength: 100, nullable: true),
                    webmaster_email = table.Column<string>(maxLength: 100, nullable: true),
                    remote_feed_url = table.Column<string>(maxLength: 255, nullable: true),
                    remote_feed_processor_use_agent_fragment = table.Column<string>(maxLength: 255, nullable: true),
                    show_recent_posts_on_default_page = table.Column<bool>(nullable: false),
                    show_featured_posts_on_default_page = table.Column<bool>(nullable: false),
                    facebook_app_id = table.Column<string>(maxLength: 100, nullable: true),
                    site_name = table.Column<string>(maxLength: 200, nullable: true),
                    twitter_publisher = table.Column<string>(maxLength: 100, nullable: true),
                    twitter_creator = table.Column<string>(maxLength: 100, nullable: true),
                    default_content_type = table.Column<string>(maxLength: 50, nullable: true, defaultValue: "html"),
                    teaser_mode = table.Column<byte>(nullable: false, defaultValue: (byte)0),
                    teaser_truncation_mode = table.Column<byte>(nullable: false, defaultValue: (byte)0),
                    teaser_truncation_length = table.Column<int>(nullable: false, defaultValue: 20),
                    default_feed_items = table.Column<int>(nullable: false, defaultValue: 20),
                    max_feed_items = table.Column<int>(nullable: false, defaultValue: 1000)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_cs_content_project", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "cs_page",
                columns: table => new
                {
                    id = table.Column<string>(maxLength: 36, nullable: false),
                    project_id = table.Column<string>(maxLength: 36, nullable: false),
                    parent_id = table.Column<string>(maxLength: 36, nullable: true),
                    parent_slug = table.Column<string>(maxLength: 255, nullable: true),
                    page_order = table.Column<int>(nullable: false),
                    title = table.Column<string>(maxLength: 255, nullable: false),
                    author = table.Column<string>(maxLength: 255, nullable: true),
                    slug = table.Column<string>(maxLength: 255, nullable: false),
                    external_url = table.Column<string>(maxLength: 255, nullable: true),
                    correlation_key = table.Column<string>(maxLength: 255, nullable: true),
                    meta_description = table.Column<string>(maxLength: 500, nullable: true),
                    meta_json = table.Column<string>(nullable: true),
                    meta_html = table.Column<string>(nullable: true),
                    content = table.Column<string>(nullable: true),
                    pub_date = table.Column<DateTime>(nullable: true),
                    last_modified = table.Column<DateTime>(nullable: false),
                    is_published = table.Column<bool>(nullable: false),
                    menu_only = table.Column<bool>(nullable: false),
                    show_menu = table.Column<bool>(nullable: false),
                    view_roles = table.Column<string>(nullable: true),
                    show_heading = table.Column<bool>(nullable: false),
                    show_pub_date = table.Column<bool>(nullable: false),
                    show_last_modified = table.Column<bool>(nullable: false),
                    show_categories = table.Column<bool>(nullable: false),
                    show_comments = table.Column<bool>(nullable: false),
                    menu_filters = table.Column<string>(maxLength: 500, nullable: true),
                    categories_csv = table.Column<string>(maxLength: 500, nullable: true),
                    disable_editor = table.Column<bool>(nullable: false),
                    content_type = table.Column<string>(maxLength: 50, nullable: true, defaultValue: "html"),
                    created_utc = table.Column<DateTime>(nullable: false),
                    created_by_user = table.Column<string>(maxLength: 100, nullable: true),
                    last_modified_by_user = table.Column<string>(maxLength: 100, nullable: true),
                    draft_content = table.Column<string>(nullable: true),
                    draft_author = table.Column<string>(maxLength: 255, nullable: true),
                    draft_pub_date = table.Column<DateTime>(nullable: true),
                    template_key = table.Column<string>(maxLength: 255, nullable: true),
                    serialized_model = table.Column<string>(nullable: true),
                    draft_serialized_model = table.Column<string>(nullable: true),
                    serializer = table.Column<string>(maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_cs_page", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "cs_page_category",
                columns: table => new
                {
                    value = table.Column<string>(maxLength: 50, nullable: false),
                    page_entity_id = table.Column<string>(maxLength: 36, nullable: false),
                    project_id = table.Column<string>(maxLength: 36, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_cs_page_category", x => new { x.value, x.page_entity_id });
                });

            migrationBuilder.CreateTable(
                name: "cs_post",
                columns: table => new
                {
                    id = table.Column<string>(maxLength: 36, nullable: false),
                    blog_id = table.Column<string>(maxLength: 36, nullable: false),
                    title = table.Column<string>(maxLength: 255, nullable: false),
                    correlation_key = table.Column<string>(maxLength: 255, nullable: true),
                    author = table.Column<string>(maxLength: 255, nullable: true),
                    slug = table.Column<string>(maxLength: 255, nullable: false),
                    meta_description = table.Column<string>(maxLength: 500, nullable: true),
                    meta_json = table.Column<string>(nullable: true),
                    meta_html = table.Column<string>(nullable: true),
                    content = table.Column<string>(nullable: true),
                    pub_date = table.Column<DateTime>(nullable: true),
                    last_modified = table.Column<DateTime>(nullable: false),
                    is_published = table.Column<bool>(nullable: false),
                    is_featured = table.Column<bool>(nullable: false),
                    image_url = table.Column<string>(maxLength: 250, nullable: true),
                    thumbnail_url = table.Column<string>(maxLength: 250, nullable: true),
                    categories_csv = table.Column<string>(maxLength: 500, nullable: true),
                    content_type = table.Column<string>(maxLength: 50, nullable: true, defaultValue: "html"),
                    auto_teaser = table.Column<string>(nullable: true),
                    teaser_override = table.Column<string>(nullable: true),
                    suppress_teaser = table.Column<bool>(nullable: false),
                    created_utc = table.Column<DateTime>(nullable: false),
                    created_by_user = table.Column<string>(maxLength: 100, nullable: true),
                    last_modified_by_user = table.Column<string>(maxLength: 100, nullable: true),
                    draft_content = table.Column<string>(nullable: true),
                    draft_author = table.Column<string>(maxLength: 255, nullable: true),
                    draft_pub_date = table.Column<DateTime>(nullable: true),
                    template_key = table.Column<string>(maxLength: 255, nullable: true),
                    serialized_model = table.Column<string>(nullable: true),
                    draft_serialized_model = table.Column<string>(nullable: true),
                    serializer = table.Column<string>(maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_cs_post", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "cs_post_category",
                columns: table => new
                {
                    value = table.Column<string>(maxLength: 50, nullable: false),
                    post_entity_id = table.Column<string>(maxLength: 36, nullable: false),
                    project_id = table.Column<string>(maxLength: 36, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_cs_post_category", x => new { x.value, x.post_entity_id });
                });

            migrationBuilder.CreateTable(
                name: "cs_page_comment",
                columns: table => new
                {
                    id = table.Column<string>(maxLength: 36, nullable: false),
                    page_entity_id = table.Column<string>(maxLength: 36, nullable: true),
                    project_id = table.Column<string>(maxLength: 36, nullable: false),
                    author = table.Column<string>(maxLength: 255, nullable: true),
                    email = table.Column<string>(maxLength: 100, nullable: true),
                    website = table.Column<string>(maxLength: 255, nullable: true),
                    content = table.Column<string>(nullable: true),
                    pub_date = table.Column<DateTime>(nullable: false),
                    ip = table.Column<string>(maxLength: 100, nullable: true),
                    user_agent = table.Column<string>(maxLength: 255, nullable: true),
                    is_admin = table.Column<bool>(nullable: false),
                    is_approved = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_cs_page_comment", x => x.id);
                    table.ForeignKey(
                        name: "fk_cs_page_comment_cs_page_page_entity_id",
                        column: x => x.page_entity_id,
                        principalTable: "cs_page",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "cs_page_resource",
                columns: table => new
                {
                    id = table.Column<string>(maxLength: 36, nullable: false),
                    page_entity_id = table.Column<string>(maxLength: 36, nullable: true),
                    sort = table.Column<int>(nullable: false),
                    type = table.Column<string>(maxLength: 10, nullable: false),
                    environment = table.Column<string>(maxLength: 15, nullable: false),
                    url = table.Column<string>(maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_cs_page_resource", x => x.id);
                    table.ForeignKey(
                        name: "fk_cs_page_resource_cs_page_page_entity_id",
                        column: x => x.page_entity_id,
                        principalTable: "cs_page",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "cs_post_comment",
                columns: table => new
                {
                    id = table.Column<string>(maxLength: 36, nullable: false),
                    post_entity_id = table.Column<string>(maxLength: 36, nullable: true),
                    project_id = table.Column<string>(maxLength: 36, nullable: false),
                    author = table.Column<string>(maxLength: 255, nullable: true),
                    email = table.Column<string>(maxLength: 100, nullable: true),
                    website = table.Column<string>(maxLength: 255, nullable: true),
                    content = table.Column<string>(nullable: true),
                    pub_date = table.Column<DateTime>(nullable: false),
                    ip = table.Column<string>(maxLength: 100, nullable: true),
                    user_agent = table.Column<string>(maxLength: 255, nullable: true),
                    is_admin = table.Column<bool>(nullable: false),
                    is_approved = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_cs_post_comment", x => x.id);
                    table.ForeignKey(
                        name: "fk_cs_post_comment_cs_post_post_entity_id",
                        column: x => x.post_entity_id,
                        principalTable: "cs_post",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "ix_cs_content_history_content_id",
                table: "cs_content_history",
                column: "content_id");

            migrationBuilder.CreateIndex(
                name: "ix_cs_content_history_content_source",
                table: "cs_content_history",
                column: "content_source");

            migrationBuilder.CreateIndex(
                name: "ix_cs_content_history_correlation_key",
                table: "cs_content_history",
                column: "correlation_key");

            migrationBuilder.CreateIndex(
                name: "ix_cs_content_history_created_by_user",
                table: "cs_content_history",
                column: "created_by_user");

            migrationBuilder.CreateIndex(
                name: "ix_cs_content_history_last_modified_by_user",
                table: "cs_content_history",
                column: "last_modified_by_user");

            migrationBuilder.CreateIndex(
                name: "ix_cs_content_history_title",
                table: "cs_content_history",
                column: "title");

            migrationBuilder.CreateIndex(
                name: "ix_cs_page_correlation_key",
                table: "cs_page",
                column: "correlation_key");

            migrationBuilder.CreateIndex(
                name: "ix_cs_page_parent_id",
                table: "cs_page",
                column: "parent_id");

            migrationBuilder.CreateIndex(
                name: "ix_cs_page_project_id",
                table: "cs_page",
                column: "project_id");

            migrationBuilder.CreateIndex(
                name: "ix_cs_page_category_page_entity_id",
                table: "cs_page_category",
                column: "page_entity_id");

            migrationBuilder.CreateIndex(
                name: "ix_cs_page_category_project_id",
                table: "cs_page_category",
                column: "project_id");

            migrationBuilder.CreateIndex(
                name: "ix_cs_page_category_value",
                table: "cs_page_category",
                column: "value");

            migrationBuilder.CreateIndex(
                name: "ix_cs_page_comment_page_entity_id",
                table: "cs_page_comment",
                column: "page_entity_id");

            migrationBuilder.CreateIndex(
                name: "ix_cs_page_comment_project_id",
                table: "cs_page_comment",
                column: "project_id");

            migrationBuilder.CreateIndex(
                name: "ix_cs_page_resource_page_entity_id",
                table: "cs_page_resource",
                column: "page_entity_id");

            migrationBuilder.CreateIndex(
                name: "ix_cs_post_blog_id",
                table: "cs_post",
                column: "blog_id");

            migrationBuilder.CreateIndex(
                name: "ix_cs_post_correlation_key",
                table: "cs_post",
                column: "correlation_key");

            migrationBuilder.CreateIndex(
                name: "ix_cs_post_slug",
                table: "cs_post",
                column: "slug");

            migrationBuilder.CreateIndex(
                name: "ix_cs_post_category_post_entity_id",
                table: "cs_post_category",
                column: "post_entity_id");

            migrationBuilder.CreateIndex(
                name: "ix_cs_post_category_project_id",
                table: "cs_post_category",
                column: "project_id");

            migrationBuilder.CreateIndex(
                name: "ix_cs_post_category_value",
                table: "cs_post_category",
                column: "value");

            migrationBuilder.CreateIndex(
                name: "ix_cs_post_comment_post_entity_id",
                table: "cs_post_comment",
                column: "post_entity_id");

            migrationBuilder.CreateIndex(
                name: "ix_cs_post_comment_project_id",
                table: "cs_post_comment",
                column: "project_id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "cs_content_history");

            migrationBuilder.DropTable(
                name: "cs_content_project");

            migrationBuilder.DropTable(
                name: "cs_page_category");

            migrationBuilder.DropTable(
                name: "cs_page_comment");

            migrationBuilder.DropTable(
                name: "cs_page_resource");

            migrationBuilder.DropTable(
                name: "cs_post_category");

            migrationBuilder.DropTable(
                name: "cs_post_comment");

            migrationBuilder.DropTable(
                name: "cs_page");

            migrationBuilder.DropTable(
                name: "cs_post");
        }
    }
}
