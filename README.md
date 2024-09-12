# AutoFileManager

This is a PoC of an executable application that reads a data file where the line is delimited by size (start and end position) and transforms it into .csv files depending on the configuration registered in the .json files (which simulate a database).

## Database (JSON)
The configuration files are located in the Data/database directory:
- **Files**:
    - `ContentType.json` - Contains the specification of the data contained in the file, description, position, size, and order
    - `InformationType.json` - Contains the types of records that the file can have

## Directory Configuration
Inside the Settings folder, there is the `ConfigurationHelper.cs` class that contains the input and output configuration of the files and the path to the json files that represent the database.

## Processing
When running the application locally or performing the publish, directories will be generated in the project's bin folder:
- **Directories**:
    - `bin/Debug/netcoreapp3.1`:
        - `Data/database` - Database files
        - `Public`:
            - `input` - Location where the file for processing should be placed
                - `PROCESSING` - All files that are being processed
                - `PROCESSED` - Processed files
                - `FAIL` - Files that failed to process
            - `output` - .csv files generated from the information contained in the processed file
                - The file name will be the type of the file plus the original name of the processed file
                  - `RecordType_originalName.csv`
