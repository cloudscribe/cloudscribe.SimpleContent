<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="3.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
	<xsl:output method="html" version="1.0" encoding="utf-8" indent="yes"/>
	<xsl:template match="/">
		<html xmlns="http://www.w3.org/1999/xhtml">
			<head>
				<title>
					<xsl:value-of select="/rss/channel/title"/>
				</title>
				<meta charset="utf-8"/>
				<meta name="viewport" content="width=device-width, initial-scale=1"/>
				<link rel="stylesheet" href="/rss.css"/>
			</head>
			<body>
				<header>
					<h1>
						<xsl:value-of select="/rss/channel/title"/>
					</h1>
					<p>
						<xsl:value-of select="/rss/channel/description"/>
					</p>
				</header>
				<main>
					<xsl:for-each select="/rss/channel/item">
						<article>
							<h3>
								<a>
									<xsl:attribute name="href">
										<xsl:value-of select="link"/>
									</xsl:attribute>
									<xsl:value-of select="title"/>
								</a>
							</h3>
							<p>
								<xsl:value-of select="description"/>
							</p>
							<p>
								<xsl:value-of select="pubDate"/>
							</p>
						</article>
					</xsl:for-each>
				</main>
			</body>
		</html>
	</xsl:template>
</xsl:stylesheet>