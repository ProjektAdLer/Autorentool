import sys
import time

from selenium import webdriver
from selenium.common.exceptions import TimeoutException, WebDriverException
from selenium.webdriver.chrome.options import Options
from selenium.webdriver.chrome.service import Service
from selenium.webdriver.common.by import By
from selenium.webdriver.support import expected_conditions as EC
from selenium.webdriver.support.ui import WebDriverWait


def test_3d(url_3d, username, password, course_name, space_name, element_name):
    # Configuration
    timeout = 20  # seconds

    service = Service('/usr/bin/chromedriver')
    options = Options()
    # options.add_argument("--headless")
    options.add_argument("--no-sandbox")
    options.add_argument("--disable-dev-shm-usage")
    options.add_argument("--disable-gpu")
    options.add_argument("--window-size=1920,1080")
    options.add_argument("--ignore-certificate-errors")
    options.add_argument("--ignore-ssl-errors")

    try:
        print("Initializing Chrome driver...")
        driver = webdriver.Chrome(service=service, options=options)
        print("Chrome driver initialized successfully")

        # Navigate to enrolment page
        print(f"Navigating to enrolment page: {url_3d}")
        driver.get(f"{url_3d}")

        print("Current page title:", driver.title)
        print("Current URL:", driver.current_url)
        # print("Page source:", driver.page_source)
        print("Page body:", driver.find_element(By.TAG_NAME, 'body').get_attribute('innerHTML'))

        print("Waiting for username field...")
        username_field = WebDriverWait(driver, timeout).until(
            EC.presence_of_element_located((By.CSS_SELECTOR, "[data-testid='userName']")))
        print("Username field found")
        username_field.send_keys(username)
        time.sleep(1)

        print("Entering password")
        password_field = driver.find_element(By.CSS_SELECTOR, "[data-testid='password']")
        password_field.send_keys(password)
        time.sleep(1)

        print("Clicking login button")
        login_button = driver.find_element(By.CSS_SELECTOR, "[data-testid='loginButton']")
        login_button.click()
        WebDriverWait(driver, timeout).until(
            EC.presence_of_element_located((By.CSS_SELECTOR, "[data-testid='logout']")))

        lernwelt_button = WebDriverWait(driver, timeout).until(
            EC.element_to_be_clickable((By.XPATH, "//button[.//p[contains(text(), 'zum Lernwelt-Menü')]]"))
        )
        lernwelt_button.click()
        print("Clicked on 'zum Lernwelt-Menü' button")

        testworld_button = WebDriverWait(driver, timeout).until(
            EC.element_to_be_clickable((By.XPATH, f"//button[contains(text(), '{course_name}')]"))
        )
        testworld_button.click()
        print(f"Clicked on {course_name} button")

        # Warten auf den "Lernwelt öffnen!" Button und darauf klicken
        print("Waiting for 'Lernwelt öffnen!' button...")
        lernwelt_oeffnen_button = WebDriverWait(driver, timeout).until(
            EC.element_to_be_clickable((By.XPATH, "//button[contains(text(), 'Lernwelt öffnen!')]"))
        )
        lernwelt_oeffnen_button.click()
        print("Clicked on 'Lernwelt öffnen!' button")

        close_button = WebDriverWait(driver, timeout).until(
            EC.element_to_be_clickable((By.XPATH, "//img[@alt='CloseButton']"))
        )
        close_button.click()
        print("Clicked on 'CloseButton'")

        testspace_button = WebDriverWait(driver, timeout).until(
            EC.element_to_be_clickable((By.XPATH, f"//button[.//p[contains(text(), '{space_name}')]]"))
        )
        testspace_button.click()
        print(f"Clicked on '{space_name}' button")

        WebDriverWait(driver, timeout).until(
            EC.element_to_be_clickable((By.XPATH, "//button[contains(text(), 'Lernraum betreten!')]")))

        print("Current page title:", driver.title)
        print("Current URL:", driver.current_url)
        # print("Page source:", driver.page_source)
        print("Page body:", driver.find_element(By.TAG_NAME, 'body').get_attribute('innerHTML'))

        page_source = driver.page_source
        if course_name in page_source and space_name in page_source and element_name in page_source:
            print(f"Found all required elements: {course_name}, {space_name}, {element_name}")
            return True
        else:
            print("Not all required elements were found on the page.")
            if course_name not in page_source:
                print(f"{course_name} not found.")
            if space_name not in page_source:
                print(f"{space_name} not found.")
            if element_name not in page_source:
                print(f"{element_name} not found.")
            return False

    except TimeoutException as e:
        print(f"Timeout occurred: {e}")
        print("Current page source:")
        # print(driver.page_source)
        print("Page body:", driver.find_element(By.TAG_NAME, 'body').get_attribute('innerHTML'))
    except WebDriverException as e:
        print(f"WebDriver exception occurred: {e}")
    except Exception as e:
        print(f"An unexpected error occurred: {e}")
    finally:
        if 'driver' in locals():
            driver.quit()
            print("Chrome driver closed")


if __name__ == "__main__":
    if len(sys.argv) != 7:
        print("Usage: test_3d.py <url_3d> <username> <password> <course_name> <space_name> <element_name>")
        sys.exit(1)

    url_3d = sys.argv[1]
    username = sys.argv[2]
    password = sys.argv[3]
    course_name = sys.argv[4]
    space_name = sys.argv[5]
    element_name = sys.argv[6]

    success = test_3d(url_3d, username, password, course_name, space_name, element_name)
    if success:
        print("Test 3d process completed successfully")
        sys.exit(0)
    else:
        print("Test 3d process failed")
        sys.exit(1)
