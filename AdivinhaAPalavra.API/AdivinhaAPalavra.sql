CREATE TABLE "words" (
    "id" int NOT NULL AUTO_INCREMENT,
    "word" varchar(5) NOT NULL,
    PRIMARY_KEY KEY ("word")
);

CREATE TABLE "matches" (
    "Id" varchar(100) NOT NULL,
    "id_word" int NOT NULL,
    PRIMARY_KEY("Id"),
    KEY "word_fk" ("id_word"),
    CONSTRAINT "word_fk" FOREIGN_KEY ("id_word") REFERENCES "words" ("id")
);