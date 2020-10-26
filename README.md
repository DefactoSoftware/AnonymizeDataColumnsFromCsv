# AnonymizeDataColumnsFromCsv

A Windows console application that expects a comma separated values file (; as delimiter) of 44 columns and anonymize the specified columns.

###Dependencies

.Net Framework 4.5

###Configuration

In the appSettings section we can configure the importfiles to a maximum of three (importfile1, importfile2, importfile3) and the corresponding exportfiles (exportfile1, exportfile2, exportfile3).
The importfile will be read into a datatable with the configured CodePage setting (CodePage key).
All the specified columns numbers in the setting 'AnonymizeColumnNumbers' will be set as an empty string and the result will be saved in the exportfile.
