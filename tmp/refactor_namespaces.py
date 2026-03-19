import os
import glob

replacements = {
    "namespace CleanArchitectureDemo.Domain": "namespace CleanArchitectureDemo.Modules.Catalog.Domain",
    "using CleanArchitectureDemo.Domain": "using CleanArchitectureDemo.Modules.Catalog.Domain",
    "namespace CleanArchitectureDemo.Application": "namespace CleanArchitectureDemo.Modules.Catalog.Application",
    "using CleanArchitectureDemo.Application": "using CleanArchitectureDemo.Modules.Catalog.Application",
    "namespace CleanArchitectureDemo.Infrastructure": "namespace CleanArchitectureDemo.Modules.Catalog.Infrastructure",
    "using CleanArchitectureDemo.Infrastructure": "using CleanArchitectureDemo.Modules.Catalog.Infrastructure"
}

# The dictionary is ordered so we do namespace/using first. 
# We need to make sure we don't accidentally replace correctly Shared.Kernel ones, 
# but they are CleanArchitectureDemo.Shared.Kernel, so they won't match.

search_dir = "src"

for root, _, files in os.walk(search_dir):
    for file in files:
        if file.endswith(".cs"):
            file_path = os.path.join(root, file)
            with open(file_path, "r", encoding="utf-8") as f:
                content = f.read()
            
            original_content = content
            for old, new in replacements.items():
                content = content.replace(old, new)
            
            if content != original_content:
                with open(file_path, "w", encoding="utf-8") as f:
                    f.write(content)
                print(f"Updated {file_path}")
print("Done")
