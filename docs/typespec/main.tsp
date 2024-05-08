import "@typespec/http";
import "@typespec/rest";
import "@typespec/openapi3";

using TypeSpec.Http;
using TypeSpec.Rest;

/** This is the DW Blog Share service */
@service({
    title: "DW Blog Share Service",
})
@server("https://api.davidwesst.com", "The service endpoint")

namespace BlogShare;

model BlogPost {
    title: string;
    description: string;
    excerpt: string;
    categories: string[];
    tags: string[];
    publishDateTimeGMT: DateTimeKnownEncoding.rfc3339;
    updatedDateTimeGMT: DateTimeKnownEncoding.rfc3339;
    htmlContent: string;
    mdContent: string;
    mediaUrls: url[];
}

@route("/blogshare/wd")
@post op ToWesternDevs(@body blogPost: BlogPost) : OkResponse | BadRequestResponse;

