CREATE INDEX IX_Company_SortName ON Company (
    Name
);

CREATE INDEX IX_Company_CityId ON Company (
    CityId
);


CREATE INDEX IX_Advisor_Lookup ON Advisor (
    CompanyId,
    FirstName,
    LastName
);


CREATE INDEX IX_Student_SpecialtyId ON Student (
    SpecialtyId
);


CREATE INDEX IX_Student_InternalAdvisorId ON Student (
    InternalAdvisorId
);


CREATE INDEX IX_Student_ExternalAdvisorId ON Student (
    ExternalAdvisorId
);


CREATE INDEX IX_Student_ReviewerAdvisorId ON Student (
    ReviewerAdvisorId
);


CREATE INDEX IX_Student_CompanyId ON Student (
    CompanyId
);


CREATE INDEX IX_Student_LastUpdated ON Student (
    UpdatedOn
);


CREATE INDEX IX_Student_BySemester_Expr ON Student (
    strftime('%Y', StartDate),
    Semester,
    FirstName,
    LastName
);
